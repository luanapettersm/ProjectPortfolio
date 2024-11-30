Cypress.Commands.addAll({
    Login(username, password) {
        cy.visit('/')
        //cy.get(':nth-child(2) > .nav-link').click()
        //cy.get('.btn').click()
        //cy.get('#cpfId').click()
        //cy.get('#cnpjId').click()
        //cy.get('#clientInfoId').click().type('02645328997')
        //cy.get('#clientInfoId').click().type('14533121000103')
        //cy.get('#nameId').click().type('Vanessa Petters')
        //cy.get('#phoneNumberId').click().type('047991593560')
        //cy.get('#mailId').click()
        //cy.get('#zipCodeId').click().type('89207760')
        //cy.get('#addressId').click().type('Rua Guilhon Ribeiro, 88 - Guanabara')
        //cy.get('#cityId').click().type('Joinville')
        //cy.get('#stateId').click().type('Santa Catarina')
        //cy.get('#isEnabledId').click()
        cy.get('#input-1').type(username)
        cy.get('#input-3').type(password)
        cy.get('.v-btn').contains('Entrar').click().wait(3500);
    }
}) 