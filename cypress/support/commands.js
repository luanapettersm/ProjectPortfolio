Cypress.Commands.addAll({
    Login(username, password) {
        cy.visit('/')

        cy.get('#input-1').type(username)
        cy.get('#input-3').type(password)
        cy.get('.v-btn').contains('Entrar').click().wait(3500);
    }
}) 