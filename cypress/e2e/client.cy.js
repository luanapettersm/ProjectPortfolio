/// <reference types="cypress" />

it('Criar client', () => {
    cy.get(':nth-child(2) > .nav-link').click()
    cy.get('.btn').click()
    cy.get('#cpfId').click()
    cy.get('#cnpjId').click()
    cy.get('#clientInfoId').click().type('02645328997')
    cy.get('#clientInfoId').click().type('14533121000103')
    cy.get('#nameId').click().type('Vanessa Petters')
    cy.get('#phoneNumberId').click().type('047991593560')
    cy.get('#mailId').click()
    cy.get('#zipCodeId').click().type('89207760')
    cy.get('#addressId').click().type('Rua Guilhon Ribeiro, 88 - Guanabara')
    cy.get('#cityId').click().type('Joinville')
    cy.get('#stateId').click().type('Santa Catarina')
    cy.get('#isEnabledId').click()
    cy.get('.mb-1').click()
});

it('Criar projeto para o client', () => {
    cy.get(':nth-child(1) > :nth-child(5) > [title="Projetos"] > .glyphicon').click()
    cy.get('.modal-body > .d-flex > .btn').click()
    cy.get('#projectId').click().type('Novo projeto para o cliente')
    cy.get('#descriptionId').click().type('Descrição do novo projeto do cliente')
    cy.get('#addressId').click().type('Address')
    cy.get('#numberId').click().type('25')
    cy.get('#cityId').clear().type('City')
    cy.get('#isEnabledId').click()
    cy.get('.mb-1').click()
});
