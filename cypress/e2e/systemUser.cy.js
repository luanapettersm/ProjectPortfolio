/// <reference types="cypress" />

it('Criar usuário', () => {
    cy.get(':nth-child(3) > .nav-link').click()
    cy.get('.btn').click()
    cy.get('#nameId').click().type('Joao')
    cy.get('#surnameId').click().type('Silva')
    cy.get('#userNameId').click().type('joao.silva')
    cy.get('#passwordId').click().type('Joinville@2024')
    cy.get('#businessRoleId').click().eq(0)
    cy.get('#isEnabledId').click()
});