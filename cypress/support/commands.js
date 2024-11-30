Cypress.Commands.addAll({
    Login(username, password) {
        cy.visit('/')
        cy.get(':nth-child(2) > .nav-link').click()
        cy.get(':nth-child(1) > :nth-child(5) > [title="Projetos"]').click().should('be.visible')
        cy.get('.modal-body > .d-flex > .btn').click()
        cy.get('#projectId').click().type('Novo projeto para o cliente')
        cy.get('#descriptionId').click().type('Descrição do novo projeto do cliente')
        cy.get('#addressId').click().type('Address')
        cy.get('#numberId').click().type('25')
        cy.get('#cityId').clear().type('City')
        cy.get('#isEnabledId').click()
        cy.get('.mb-1').click()
        cy.get('#input-1').type(username)
        cy.get('#input-3').type(password)
        cy.get('.v-btn').contains('Entrar').click().wait(3500);
    }
}) 