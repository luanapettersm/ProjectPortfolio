Cypress.Commands.addAll({
    Login(email, password) {
        cy.visit('/')

        cy.get('#input-1').type(email)
        cy.get('#input-3').type(password)
        cy.get('.v-btn').contains('Entrar').click().wait(3500);
    },
    //SelectInput(label, option, position = 0) {
    //    cy.get([label = "${label}"][aria - owns]: visible).then((select) => {
    //        if (position < 0) position = select.length - position;
    //        else if (select.length <= position) position = select.length - 1;
    //        cy.get(select).eq(position).click().invoke('attr', 'aria-owns').as('response');
    //        cy.get('@response').then((response) => {
    //            cy.get('#' + response).contains(option).click();
    //        });
    //    });
    //},
}) 