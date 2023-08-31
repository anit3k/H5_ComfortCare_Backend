<a name="readme-top"></a>

# Comfort Care.

Backend for the app Comfort care.

## Description

This solution contains both the domain layer, service layer, data layer and our API.

## Table of contents
<h1><a href="ComfortCare.Api">GATEWAY</a></h1>
<ul>
    <li style="list-style: none;">
        <a href="ComfortCare.Api/Controllers">Controller</a>
        <ul>
            <li style="list-style: none;">
                <a href="ComfortCare.Api/Controllers/LoginController.cs">Login Controller</a>
                <p style="margin: 0; padding: 0;">Her kan du finde vores login controller, som håndtere brugerens login informationer.</p>
            </li>
            <li style="list-style: none;">
                <a href="ComfortCare.Api/Controllers/PeriodController.cs">Period Controller</a>
                <p style="margin: 0; padding: 0;">Her kan du finde vores Period controller, som er med til at generere ruter, og diverse.</p>
            </li>
        </ul>
    </li>
</ul>
<h1><a href="ComfortCare.Service">Service</a></h1>
<ul>
    <li style="list-style: none;">
        <a href="ComfortCare.Service/PeriodService.cs">Period Service</a>
        <p style="margin: 0; padding: 0;">Her kan du finde vores Periode Service, som opretter arbejdsperiode.</p>
    </li>
    <li style="list-style: none;">
        <a href="ComfortCare.Service/UserService.cs">User Service</a>
        <p style="margin: 0; padding: 0;">Her kan du finde vores User Service, som står for håndetering af brugerne.</p>
    </li>
</ul>
<h1><a href="ComfortCare.Data">Data</a></h1>
<ul>
    <li>
        <a href="ComfortCare.Data/ComfortCareRepository.cs">Repository</a>
        <p style="margin: 0; padding: 0;">Her kan du finde vores repository håndtering som er her vi laver vores database kald.</p>
    </li>
    <li>
        <a href="ComfortCare.Data/ComfortCareDbContext.cs">Context</a>
        <p style="margin: 0; padding: 0;">Her kan du finde vores Context, som er er genereret igennem EF Core Power Tools.</p>
    </li>
    <li>
        <a href="ComfortCare.Data/Models">DB Models</a>
        <p style="margin: 0; padding: 0;">Her kan du finde vores alle modeller benyttet i vores database, sat op til Entity Framework.</p>
    </li>
</ul>
<h1><a href="ComfortCare.Domain">Domain</a></h1>
<ul>
    <li>
        <a href="ComfortCare.Domain/BusinessLogic/PeriodManager.cs">Period Manager</a>
        <p style="margin: 0; padding: 0;">Her kan du finde vores Periode manager.</p>
    </li>
    <li>
        <a href="ComfortCare.Domain/BusinessLogic/RouteGenerator.cs">Route Generator</a>
        <p style="margin: 0; padding: 0;">Her kan du finde vores logik til beregning af daglige ruter</p>
    </li>
    <li>
        <a href="ComfortCare.Domain/BusinessLogic/SchemaGenerator.cs">Schema Generator</a>
        <p style="margin: 0; padding: 0;">Her kan du finde vores logik for skema generering. Det er her vi samler alle de forrige udregner til at kunne danne et skema for medarbejdere.</p>
    </li>
</ul>
<p align="right"><a href="#readme-top">back to top</a></p>
