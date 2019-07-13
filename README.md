# Alloy-Optimizer--GA
The Sirius Alloy Company specializes in melting metals in certain ratios and selling these alloys to the manufacturing industry.  They produce four main alloys each consisting of different ratios of platinum, iron and copper. The selling price and the amount of electricity used (in kWh) to produce one kilogram of each alloy is displayed in Table 1, along with the percentages platinum, iron and copper of which the alloys consist.

Alloy	Selling Price/kg	Platinum	Iron 	Copper	Electricity (kWh/kg)
Adamantium	R3000	20%	70%	10%	25
Unobtainium	R3100	30%	20%	50%	23
Dilithium	R5200	80%	10%	10%	35
Pandemonium	R2500	10%	50%	40%	20
Table 1

The cost per kilogram of the raw metals are:
Platinum: R1200 + R10*(Total Nr of platinum kilograms)
Iron:	R300
Copper: R800

If more than 8 kilograms of copper is bought, a 10% discount is given on the price for copper. For every kilogram of platinum bought, Sirius Alloy receives a kilogram of iron for free.  

Due to the electricity crisis, Eskom charges an exponential price for electricity used (i.e. the first few kWh are cheap, but it becomes more expensive the more is used).  The cost of electricity, in Rand, is given by the following formula:
Cost = e0.005*(Total kWh)
It should be clear from the above equation that at some point the electricity costs will become larger than the profit made by the company, so there is a limit on the number of kilograms of alloy that the company can profitably produce.

Write a Genetic Algorithm that calculates the amounts (in kilograms) of each of the four elements that must be produced in order to maximize profits for the Sirius Alloy Company.

Complete the submission template for this assignment.  Try to keep the completed template to two pages. Attach your code to your submission.
