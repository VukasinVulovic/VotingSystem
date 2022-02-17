function main() {
    const app = document.querySelector('#app');
    const form = document.querySelector('#app .user-form');

    let busy = false;

    const spinner = new Spinner();
    app.appendChild(spinner.HTML);

    form.addEventListener('submit', async e => {
        e.preventDefault();

        if(busy)
            return;

        const msg = await parseForm(form);
        
        if(!msg)
            return;

        const com = new Communication();

        com.on('error', async err => {
            new ErrorModal('Sistemska Greška', 'Greška pri pristupu sitema, probajte ponovo.');

            spinner.reset();
            com.dispose();
            busy = false;
        });
    
        com.on('response', async message => {
            spinner.update(100, 'Dekriptovanje informacija podataka');

            await delay(1000);
            spinner.reset();
            
            const token = message.token;
            
            if(!token)
                return new ErrorModal('Sistemska Greška', 'Greška pri čuvanju privremenog ključa, probajte ponovo.');
    
            com.dispose();
            busy = false;

            location.replace('/?token=' + token);
        });
        
        com.on('ready', async() => {
            spinner.show();
            
            spinner.update(0, 'Učitavanje');
            await delay(100);

            spinner.update(25, 'Slanje podataka');
            await delay(1000);

            spinner.update(50, 'Provera podataka');
            com.sendMessage('/login_voter', msg);
        });
        
        com.load();
        busy = true;
    });
}

window.addEventListener('load', () => {    
    setRequirement(document.getElementsByName('voter_id')[0], 4, 'pin');
    setRequirement(document.getElementsByName('voter_password')[0], 20, 'password');
    main();
});