/// <reference types="cypress" />

const doc = Array.from({ length: 11 }, () => Math.floor(Math.random() * 100))

it('Testa adicionar dependente', () => {
    cy.get('.v-btn').contains('Adicionar Dependente').click();
    cy.get('[data-cy="CPF"]: visible').clear().click().type(doc);
    cy.get('[data-cy="Nome"]: visible').clear().click().type('teste');
    cy.get('[data-cy="dtNascimento"]:visible')
});