const form = document.getElementById('form')

const firstname_input = document.getElementById('firstname-input')
const email_input = document.getElementById('email-input')
const password_input = document.getElementById('password-input')
const repeat_password_input = document.getElementById('repeat-password-input')

const error_message = document.getElementById('error-message')

form.addEventListener('submit', (e) => {
    let errors = []

    if(firstname_input){
        errors = getSignupFormErrors(password_input.value, repeat_password_input.value)
    } else {
        errors = getLoginFormErrors(email_input.value, password_input.value)
    }

    if (errors.length > 0){
        e.preventDefault()
        error_message.innerText = errors.join(". ")
    }
})

function getSignupFormErrors(password, repeatPassword){
    let errors = []
    
    if (password !== repeatPassword){
        errors.push('Password Does Not Match Repeated Password')
        password_input.parentElement.classList.add('incorrect')
        repeat_password_input.parentElement.classList.add('incorrect')
    }

    return errors
}

const allInputs = [firstname_input, email_input, password_input, repeat_password_input]
allInputs.forEach(input =>{
    input.addEventListener('input', () => {
        if(input.parentElement.classList.contains('incorrect')){
            input.parentElement.classList.remove('incorrect')
            error_message.innerText =  ''
        }
    })
})
