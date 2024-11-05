document.querySelectorAll('.toggle-password').forEach(item => {
    item.addEventListener('click', function() {
        const passwordInput = this.nextElementSibling;
        const icon = this.querySelector('ion-icon');
        
        if (passwordInput.type === 'password') {
            passwordInput.type = 'text';
            icon.setAttribute('name', 'lock-open-outline');
        } else {
            passwordInput.type = 'password';
            icon.setAttribute('name', 'lock-closed');
        }
    });
});

const wrapper = document.querySelector('.wrapper');
const loginLink = document.querySelector('.login-link');
const registerLink = document.querySelector('.register-link');
const btnPopup = document.querySelector('.btmLogin-popup');
const iconClose = document.querySelector('.icon-close');

registerLink.addEventListener('click', () => {
    wrapper.classList.add('active');
});

loginLink.addEventListener('click', () => {
    wrapper.classList.remove('active');
});

btnPopup.addEventListener('click', () => {
    wrapper.classList.add('active-popup');
});

iconClose.addEventListener('click', () => {
    wrapper.classList.remove('active-popup');
});