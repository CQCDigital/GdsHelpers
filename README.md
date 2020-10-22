# GDS Helpers  · [![Build Status](https://dev.azure.com/CQCDigital/SYE-Project/_apis/build/status/GFC-MASTER-Build)](https://dev.azure.com/CQCDigital/SYE-Project/_apis/build/status/GFC-MASTER-Build)

## Table of Contents
 - [Introduction](#introduction)
 - [Project Overview](#project-overview)
 - [Examples](#examples)
 - [Technology](#technology)
 - [Licence](#licence)

## Introduction
GDSHelpers is a Nuget package that is used by CQC (Care Quality Commission) to help generate consistent [GDS Design System]([https://design-system.service.gov.uk/](https://design-system.service.gov.uk/)) compliant html.

[Back to Top](#table-of-contents)

## Project Overview
The solution is made up of 3 projects:
 - GDSHelpers - Nuget Package files
 - GDSHelpers.Tests - Unit Tests for the Nuget Package
 - GDSHelpers.TestSite - A demo test site showcasing how to use the Nuget package

[Back to Top](#table-of-contents)

## Examples
Examples are best understood when compared side by side with the [GDS Design System]([https://design-system.service.gov.uk/](https://design-system.service.gov.uk/)):

### GDS Button
GDSHelpers Nuget Syntax
```
<button gds gds-start>Start button</button>
```
Html Output
```
<button class="govuk-button govuk-button--start" data-module="govuk-button">
	Start button
	<svg class="govuk-button__start-icon" xmlns="http://www.w3.org/2000/svg" width="17.5" height="19" viewBox="0 0 33 40" aria-hidden="true" focusable="false">
	<path fill="currentColor" d="M0 0h13l20 20-20 20H0l20-20z" />
	</svg>
</button>
```

### Headers
```
<h1 gds>GDS header 1</h1>
```
HtmlOutput
```
<h1 class="govuk-heading-xl">GDS header 1</h1>
```
Available size overrides are:
```
@GdsSize.Small (Normally h4)
@GdsSize.Medium (Normally h3)
@GdsSize.Large (Normally h2)
@GdsSize.ExtraLarge (Normally h1)
```
E.g.
```
<h1 gds gds-size="@GdsSize.Small">GDS header 1 - with small override</h1>
```

[Back to Top](#table-of-contents)

## Technology
The GFC project utilises the following technology:

* **.Net Core 3.1** - .Net Core 3.1 is, at the time of writing, the latest version of Microsoft's open source .Net Framework. The GFC application has been written to be open source from the ground up.
* **GovUK Design System** - We use this design system to make our service consistent with GOV.UK. The [GovUK Design System](https://design-system.service.gov.uk/) has been designed from the research and experience of other service teams and helps us to avoid repeating work that’s already been done.

[Back to Top](#table-of-contents)

## Licence

Unless stated otherwise, the codebase is released under the MIT License. This covers both the codebase and any sample code in the documentation. The documentation is &copy; Crown copyright and available under the terms of the [Open Government 3.0 licence](http://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/).