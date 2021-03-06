(function () {

    "use strict";

    angular
        .module("geocms")
        .constant("Texts", {
            common: {
                CUSTOMERS: {
                    "en": "Customers",
                    "fr": "Clients"
                },
                IMPORT_POI: {
                    "en": "Import POI",
                    "fr": "Importer POIs"
                },
                EXPORT_POI: {
                    "en": "Export POI",
                    "fr": "Exporter POIs"
                },
                LOGOUT: {
                    "en": "Logout",
                    "fr": "D&eacute;connecter"
                },
                CMS: {
                    "en": "CMS",
                    "fr": "CMS"
                },
                USERS: {
                    "en": "Users",
                    "fr": "Utilisateurs"
                }
            },
            cms: {
                BOUNDARIES: {
                    "en": "Boundaries",
                    "fr": "Communes"
                },
                CUSTOMERS: {
                    "en": "Customers",
                    "fr": "Clients"
                },
                SEARCH: {
                    "en": "Search",
                    "fr": "Recherche"
                },
                NO_CUSTOMERS_FOUND: {
                    "en": "No customers found. Change boundary or modify your search query.",
                    "fr": "Aucun client correspond aux crit&egrave;res demand&eacute;s. Veuillez modifier votre recherche ou changer de commune."
                },
            },
            "import": {
                SELECT_FILE: {
                    "en": "Please select the file you wish to import:<br/>",
                    "fr": "<p>Veuillez choisir un fichier &agrave; importer. Le fichier doit &ecirc;tre en format CSV.</p><br/>" +
                            "Il peut contenir les colonnes suivantes: <br/>" +
                            "[Num&eacute;ro de T&eacute;l&eacute;phone] ou [NUM_ADSL] (Requis)<br/>" +
                            "[Coordon&eacute;es GPS Longitude] (Optionel)<br/>" +
                            "[Coordon&eacute;es GPS Latitude] (Optionel)<br/>" +
                            "[Photo de l'entr&eacute;e] (Optionel)<br/>" +
                            "[NOM] (Optionel)<br/>" +
                            "[LOGIN] (Optionel)<br/>" +
                            "[FORMULE] (Optionel)<br/>" +
                            "[DEBIT] (Optionel)<br/>" +
                            "[DATE_EXPIRATION] (Optionel)<br/>" +
                            "[ETAT] (Optionel) "
                },
                IMPORT_ACTION: {
                    "en": "Import",
                    "fr": "Importer"
                },
                IMPORT_SUCCESS: {
                    "en": "The file has been successfully imported.",
                    "fr": "Le fichier a &eacute;t&eacute; import&eacute; avec succ&egrave;s."
                },
                IMPORT_FAILURE: {
                    "en": "Failed when trying to import the file. ",
                    "fr": "Erreur lors de l'importation du fichier. "
                }
            },
            security: {
                USERNAME: {
                    "en": "Username",
                    "fr": "Nom d'utilisateur"
                },
                PASSWORD: {
                    "en": "Password",
                    "fr": "Mot de passe"
                },
                SUBMIT: {
                    "en": "Submit",
                    "fr": "Soumettre"
                }
            }            
    });

})();