import SystemUserPage from "@/Views/SystemUser/Index.cshtml";

/// <reference types="cypress" />

describe("SystemUserPage", () => {
    it('Criar usuário', () => {
        cy.mount(SystemUserPage);

        cy.get(':nth-child(3) > .nav-link').click()
        cy.get('.btn').click()
        cy.get('#nameId').click().type('Joao')
        cy.get('#surnameId').click().type('Silva')
        cy.get('#userNameId').click().type('joao.silva')
        cy.get('#passwordId').click().type('Joinville@2024')
        cy.get('#businessRoleId').select().eq(0)
        cy.get('#isEnabledId').click()
    });
});