const app = document.querySelector('#app');
const spinner = new Spinner();
let busy = false;

function getCandidates() {
    return new Promise((resolve, reject) => {
        fetch('/candidates')
        .then(async res => {
            if(res.status != 200)
                return reject(new Error('Server responded with: ' + res.status + ', ' + res.statusText + '.')); 
            
            res.json().then(resolve, reject);
        }, reject);
    });
}

async function castVote(index) {
    const token = urlArg('token');
    
    if(!checkToken(token))
        location.replace('/login');
    
    if(busy)
        return;

    const com = new Communication();
    
    com.on('error', async err => {
        new ErrorModal('Sistemska Greška', 'Greška pri pristupu sitema, probajte ponovo.');

        spinner.reset();
        com.dispose();
        busy = false;
    });

    com.on('response', async (message, status) => {
        spinner.update(100, 'Dekriptovanje informacija podataka');

        await delay(1000);
        spinner.reset();

        if(status === 403)
            return location.replace('/warning');

        if(status === 200)
            return location.replace('/success');
        
        const token = message.token;
        
        if(!token)
            return new ErrorModal('Sistemska Greška', 'Greška pri čuvanju privremenog ključa, probajte ponovo.');

        com.dispose();
        busy = false;
    });
    
    com.on('ready', async() => {
        spinner.setOpacity(0.3);
        spinner.show();
        
        spinner.update(0, 'Učitavanje');
        await delay(100);

        spinner.update(25, 'Slanje podataka');
        await delay(1000);

        spinner.update(50, 'Provera podataka');

        com.sendMessage('/vote', { 
            token: token, 
            option: index + 1 + '' 
        });
    });
    
    com.load();
    busy = true;
}

async function main() {
    if(!checkToken(urlArg('token')))
        location.replace('/login');

    await spinner.show();
    app.appendChild(spinner.HTML);

    await spinner.update(0, 'Učitavanje kandidata');
    await delay(1000);
    await spinner.update(50, 'Učitavanje kandidata');

    getCandidates()
    .then(async canidates => {
        await spinner.update(80, 'Učitavanje kandidata');
        await delay(100);
        canidates.forEach((canidate, i) => createChoice(canidate['title'], canidate['description'], () => castVote(i)));
        await spinner.update(100, 'Učitavanje kandidata');
        await delay(100);
        spinner.reset();
    }, console.error);

}

window.onload = main;