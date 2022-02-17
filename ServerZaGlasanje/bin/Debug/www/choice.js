const choices = document.querySelector('#app .choices');
const choices_title = document.querySelector('#app .title');

function createChoice(title_text, description_text, cb) {
    const choice = document.createElement('div');
    choice.className = 'choice';

    const front = document.createElement('div');
    front.className = 'front';

    const title = document.createElement('h5');
    title.className = 'title';

    const back = document.createElement('div');
    back.className = 'back';
    
    const info = document.createElement('p');
    info.className = 'info';
    
    front.appendChild(title);
    back.appendChild(info);
    choice.append(front, back);
    
    choices.appendChild(choice);

    title.innerText = title_text;
    info.innerText = description_text;
    
    let t = null;
    
    choice.addEventListener('mouseover', () => {
        if(choice.getAttribute('data-disabled') === 'true')
            return;

        front.style.transform = 'rotateX(180deg)';
        back.style.transform = 'rotateX(0deg)';
    
        if(t)
            clearTimeout(t);
    
        t = null;
    });
    
    choice.addEventListener('mouseout', () => {
        if(choice.getAttribute('data-disabled') === 'true')
            return;

        t = setTimeout(() => {
            front.style.transform = 'rotateX(0deg)';
            back.style.transform = 'rotateX(180deg)';
        }, 500);
    });

    choice.addEventListener('click', async() => {
        cb();

        if(choice.getAttribute('data-disabled') === 'true')
            return;

        front.style.transform = 'rotateX(0deg)';
        back.style.transform = 'rotateX(180deg)';

        choices_title.fontSize = '12rem';

        document.querySelectorAll('#app .choices .choice').forEach(c => {
            if(c != choice) {
                c.querySelector('.front .title').style.textDecoration = 'line-through';
                c.style.opacity = 0.1;
            }
            else
                c.querySelector('.front .title').style.fontSize = '3rem';

            c.setAttribute('data-disabled', 'true');
        });

        await lerp(0, 1, 0.01, 5, v => choices_title.style.opacity = 1-v);
        choices_title.innerHTML = 'Glasali ste za: ';
        await lerp(0, 1, 0.01, 5, v => choices_title.style.opacity = v);
    });
}

async function lerp(from, to, ammount, interval, cb) {
    return new Promise(resolve => {
        const loop = setInterval(() => {
            if((from += ammount) >= to) {
                resolve();
                clearInterval(loop);
            }

            cb(from);
        }, interval);
    });
}