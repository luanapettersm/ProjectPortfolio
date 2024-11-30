Cypress.Commands.addAll({
    Login(username, password) {
        cy.visit('/')

        cy.get('#userId').type(username)
        cy.get('#passwordId').type(password)
        cy.get('.btn').contains('Entrar').click().wait(3500);
    }
}) 