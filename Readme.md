# Cumulus
## Overview
This .Net core project aims to provide a simple way developers can start-up / shutdown multiple resources as part of their daily cycle.  Many companies
implement automatic "snooze" facilities which shutdown, then startup VMs to balance saving costs with the fear of inconviencing their developers.  Experiance has 
shown that companies can improve savings by implementing auto-shutdown with manual startup, contributing factors include:
- Not starting resources for Engineers who are on holiday or sick
- Engineers may have mulitple environments that they are assigned - they can only work on one
- Not starting resources for Engineers who have left or been reassigned (these should still be captured and tidied by Starters / Leavers / Movers process)

Ideally of course these scripts would not be required - Engineers could simply spin up and destroy environments as required but they are practical as well as
human reality barriers to getting there quickly for most organisations.
## Usage
### Tags
Tag Azure VMs with the following Tag:

**Tag Name:** *Cumulus*

**Tag Value:** *Project:<project name>*

### Command Line
`cumulus project list`

This will list all the projects found with a `Cumulus` tag

`cumulus project start tiger`

This will start all the VMs tagged `Project:tiger`

`cumulus project stop tiger`

This will stop all the VMs tagged `Project:tiger`

## Security
This tool acts in the context of the user who runs it, if they have - VM Operator rights or above on the machines with the correct tags the tool will start / stop them.  If not it will fail.  

## Ideas for the future
- Support Startup Order
- Handle all resources in a resource group rather than having to assing the tag to multiple VMs in the same group
- Scale down Azure SQL
- Scale down AKS
