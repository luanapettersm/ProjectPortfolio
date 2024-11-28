const { defineConfig } = require("cypress");

module.exports = defineConfig({
    e2e: {
        baseUrl: "https://localhost:5001/",
        setupNodeEvents(on, config) {
            // implement node event listeners here
        },
    },
});