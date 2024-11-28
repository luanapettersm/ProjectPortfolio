import './commands'

beforeEach(() => {
    cy.fixture('auth').then(auth => {
        const { email, password } = auth
        cy.Login(email, password)
    })
})