Cypress.Commands.addAll({
    Login(email, password) {
        cy.visit('/')
        cy.get('#input-1').type(email)
        cy.get('#input-3').type(password)
        cy.get('.v-btn').contains('Entrar').click().wait(3500);
    }
}) 