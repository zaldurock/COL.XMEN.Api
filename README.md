# COL.XMEN

The initial project is COL.XMEN.Api is a web api with the next endpoints. In the application.settings you can found the configuration to database

# Mutant

The url is https://localhost:5001/api/v1/mutant on Azure is https://colxmenapi20210226014027.azurewebsites.net/api/v1/mutant


This method is a POST 

BODY:  application/json

{
    "dna" : ["GTGCGA","CAGTGC","TTATGT","AGAAGG","CAACTA","TCACTG"]
}

RESPONSE:

{
    "dna": [
        "GTGCGA",
        "CAGTGC",
        "TTATGT",
        "AGAAGG",
        "CAACTA",
        "TCACTG"
    ],
    "dnaAll": "GTGCGACAGTGCTTATGTAGAAGGCAACTATCACTG",
    "isMutant": true,
    "id": "1d169183-9604-4b2e-aa8c-b7226660c6ab",
    "createdDate": "2021-02-26T00:29:42.9706036-05:00",
    "updatedDate": null
}


# Stats

The url is https://localhost:5001/api/v1/mutant/stats on Azure is https://colxmenapi20210226014027.azurewebsites.net/api/v1/mutant/stats

This method is a GET

RESPONSE:

{
    "count_mutant_dna": 1,
    "count_human_dna": 0,
    "ratio": 1
}
_____________________________


The application is publish in Azure with the next url.

# Mutant
https://colxmenapi20210226014027.azurewebsites.net/api/v1/mutant


# Stats
https://colxmenapi20210226014027.azurewebsites.net/api/v1/mutant/stats
