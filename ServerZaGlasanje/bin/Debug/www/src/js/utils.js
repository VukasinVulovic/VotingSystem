const delay = ms => new Promise(resolve => setTimeout(resolve, ms));
const urlArg = arg => new URLSearchParams(location.search).get(arg);
const checkToken = token => token && (new RegExp(/^[a-z0-9]{64}$/)).test(token); 

class Spinner {
    #rot = 0;
    #proc_element = null;
    #info_element = null;
    #text_element = null;

    constructor() {
        this.finished = false;
        this.value = 0;
        this.HTML = null;
        this.#init();
    }

    #init() {
        const loader = document.createElement('div');
        const info = document.createElement('div'); 
        const text = document.createElement('p'); 
        const proc = document.createElement('b');

        loader.style = 'width: 20rem; height: 20rem; background: linear-gradient(to right, #E36387, #8BCDCD); border-radius: 50%; padding: 1rem; position: absolute; display: flex; top: 20%; z-index: 1;';
        info.style = 'width: 100%; height: 100%; border-radius: 50%; background-color: #3c3f42; font-size: 1rem; position: relative; display: flex; justify-content: center; align-items: center; flex-direction: column; column-gap: 0;';
        text.style = 'font-size: 1.6rem;';
        proc.style = 'font-size: 2.5rem;';

        text.innerText = 'Učitavanje...';
        proc.innerText = '0%';

        this.#text_element = text;

        info.append(text, proc);
        loader.appendChild(info);
        this.#proc_element = proc;

        loader.style.visibility = 'hidden';

        this.HTML = loader;
        this.#info_element = info;
    }

    setOpacity(opacity) {
        this.HTML.style.opacity = opacity;
    }

    #animationLoop() {
        if(this.finished)
            return;
            
        if(this.#rot > 360)
            this.#rot = 0;
    
        this.HTML.style.transform = 'rotate(' + (this.#rot += 10) + 'deg)';
        this.#info_element.style.transform = 'rotate(' + -this.#rot + 'deg)';
        window.requestAnimationFrame(this.#animationLoop.bind(this));
    }

    async update(proc, info) {
        this.#text_element.innerText = info;
        
        while(this.value <= proc) {
            await delay(10);
            this.#proc_element.innerText = this.value + '%';
            this.value++;
        }
    }

    show() {
        this.finished = false;
        window.requestAnimationFrame(this.#animationLoop.bind(this));
        this.HTML.style.visibility = 'unset';
    }

    reset() {
        this.finished = true;
        this.HTML.style.visibility = 'hidden';
        this.value = 0;
        this.#text_element.innerText = 'Učitavanje...';
        this.#proc_element.innerText = '0%';
        // window.requestAnimationFrame(this.#animationLoop.bind(this));
    }

    dispose() {
        this.finished = true;
        loader.remove();
        delete this;
    }
}

class ErrorModal {
    constructor(title, text) {
        this.text = text;
        this.title = title;
        this.#init();
    }

    #init() {
        const cover = document.createElement('div');
        const modal = document.createElement('div');
        const title = document.createElement('h2');
        const text = document.createElement('p');
        const buttons = document.createElement('div');
        const ok_button = document.createElement('button');

        cover.style = 'width: 100%; height: 100%; top: 0; left: 0; position: absolute; background-color: rgba(0, 0, 0, 0.5); z-index: 1;';
        modal.style = 'position: absolute; padding: 0.5rem; padding-left: 1rem; padding-right: 1rem; margin: 0; display: flex; justify-content: center; align-items: center; background-color: rgba(18, 23, 27, 255); z-index: 1; border: 1px solid #da12129f; flex-flow: column; width: fit-content; border-radius: 0.7rem; opacity: 0; top: -2%; left: 50%; transform: translate(-50%, 0); transition: 1s;';
        title.style = 'font-size: 1.2rem; text-align: center; color: #da1212d2; margin: 0;';
        text.style = 'font-size: 0.7rem; text-align: center; margin: 0;';
        buttons.style = 'text-align: center; justify-content: center; display: block; align-items: center;';
        ok_button.style = 'font-size: 0.7rem; background-color: rgb(65, 65, 65); border: none; border-radius: 10%;  padding-left: 0.5rem; padding-right: 0.52rem; padding-top: 0.3rem; padding-bottom: 0.2rem; color: #d4d1d1;';

        ok_button.addEventListener('mouseover', () => {
            ok_button.style.filter = 'brightness(1.2)';
            ok_button.style.border = '1px solid rgba(93, 93, 93, 0.589)';
        });

        ok_button.addEventListener('mouseleave', () => {
            ok_button.style.filter = 'brightness(1)';
            ok_button.style.border = 'none';
        });

        title.innerText = this.title;
        text.innerText = this.text;
        
        ok_button.innerText = 'OK';
        
        buttons.append(ok_button);
        modal.append(title, text, buttons);
    
        document.body.append(cover, modal);
        
        ok_button.addEventListener('click', () => {
            cover.remove();
            modal.remove();
            delete this;
        });

        setTimeout(() => {
            modal.style.top = '25%';
            modal.style.opacity = 1;
        }, 10);
    }
}

class Communication {
    #encrypt = null;
    #cbs = {
        'ready': () => {},
        'error': console.error,
        'response': console.log
    }

    constructor() {
        this.publicKey = null;
        this.ready = false;
    }

    on(ev, cb) {
        if(!this.#cbs[ev])
            throw new Error('No event ' + ev + '.');
    
        this.#cbs[ev] = cb;  
    }

    async load() {
        this.#encrypt = new JSEncrypt();
        
        const res = await fetch('/public_key', {
            method: 'GET',
            headers: {
                'Accept': 'text/plain'
            }
        });
    
        if(res.status != 200)
            return this.#cbs['error'](new Error(`Server responded with ${res.status}: "${res.statusText}";`));
        
        try {
            this.publicKey = await res.text();
            this.#encrypt.setPublicKey(this.publicKey);
        } catch(err) {
            return this.#cbs['error'](new Error('Could not set public encryption key.'));
        }

        this.ready = true;
        this.#cbs['ready']();
    }

    async sendMessage(url, message) {
        message = typeof(message) === 'object' ? JSON.stringify(message) : message;
        const shared_key = new Array(48).fill(0).map(() => 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'[Math.floor(Math.random() * 62)]).join('');
        
        if(!this.ready)
            return this.#cbs['error'](new Error('Communication is not ready.'));
        
        const encrypted = this.#encrypt.encrypt(JSON.stringify({
            message,
            shared_key
        }));
        
        if(!encrypted)
            return this.#cbs['error'](new Error('Could not encrypt message;'));
        
        const res = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'text/plain'
            },
            body: encrypted.toString()
        });
        
        if(res.status != 200)
            return this.#cbs['error'](new Error(`Server responded with ${res.status}: "${res.statusText}";`));

        const response_message = CryptoJS.AES.decrypt(await res.text(), CryptoJS.enc.Utf8.parse(shared_key.slice(16)), { iv: CryptoJS.enc.Utf8.parse(shared_key.slice(0, 16)) }).toString(CryptoJS.enc.Utf8);
        this.#cbs['response'](JSON.parse(response_message), res.status);
    }

    dispose() {
        this.#cbs = {
            'ready': () => {},
            'error': () => {},
            'response': () => {}
        };

        delete this;
    }
}

function setRequirement(input_element, max_length, type) {
    const warning = input_element.parentElement.querySelector('.warning-icon');

    input_element.addEventListener('keydown', e => {
        if(type === 'pin' && ((e.keyCode < 48 || e.keyCode > 57) && !(e.keyCode > 96 && e.keyCode < 105)) && e.key != 'Backspace')
            e.preventDefault();

        if(input_element.value.length >= max_length && e.key != 'Backspace')
            e.preventDefault();
    });

    input_element.addEventListener('keyup', () => {
        warning.style.visibility = checkByType(input_element.value, type) ? 'hidden' : 'unset';
    });
}

function checkByType(value, type) {
    switch(type) {
        case 'pin':
            return value.length === 4;
    
        case 'password':
            return value.match(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z]{8,}$/)?.index !== undefined;

        default:
            throw new Error('Unknown type.');
    }
}

function parseForm(form) {
    return new Promise(resolve => {
        const fields = {}

        for(const e of form.querySelectorAll('input')) {
            const warning = e.parentElement.querySelector('.warning-icon');
            
            if(!warning || warning.style.visibility !== 'hidden') {
                new ErrorModal('Pogrešno unet podatak', warning?.title);
                return resolve(false);
            }
            
            fields[e.name] = e.value;
        }
    
        resolve(JSON.stringify(fields));
    });
}