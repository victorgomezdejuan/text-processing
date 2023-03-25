# Text Processing
Kata got from https://www.codurance.com/katalyst/text-processing.

Developed with dotnet (c#) and Visual Studio.

## Practice objectives
- TDD
- NUnit
- Clean code
- Github Copilot

## Brief explanation
### Requirements
As a developer that writes blog posts I want a tool that helps me to understand better the text I am writing. For that I need a way to know the following:

What are the most common words used in the text?
How many characters does the text have?
interface Processor {
   analyse(text: string);
}
The usage of such interface is not required.