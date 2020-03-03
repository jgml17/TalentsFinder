Dear Jose,

The Israeli Mossad, due to it's special recruitment methods, has access to all developers in the world.
Their cyber units are constantly growing, and need to provide support to various types of operations and technologies.
The challenge is to filter out and find the best matching candidates for the right position.

Your task will be to craft a Xamarin app, that will allow the Mossad recruiters to handle the vast amount of data on both ends.

### The Mossad recruiter is expecting to be able to:
1. Insert the match criteria (search parameters), and have them stored in the app until he changes them, even if the app is closed
2. In a seperate screen - Swipe left or right (tinder style) on a candidate, which will mark him/her as accepted or rejected
3. When a candidate is accepted, the device will vibrate
4. The Mossad doesn't give second chances. Regardless of accepting or rejecting a candidate, he/she should never be shown again
5. Later there will be developed an interface to view the accepted candidates, but this is out of scope for this task

### API
Get technologies:  
https://v1.ifs.aero/technologies/  
Get candidates:  
https://v1.ifs.aero/candidates/

### A few things to notice
1. A match criteria (search parameter) is a combination of a technology + years of experience
2. Unfortunately all Mossad backend engineers are on a vacation, filtering must be done in the app
3. The data set of candidates is vast and quickly growing, the app should not slow down with growth of the list to 100k records
4. The end points might seem static, but the data might as well change any time, make sure to use fresh data all the time
5. No need to display all data from endpoints, you choose only what you like - keep it simple
6. The app must compile to both iOS and Windows
7. User friendly UI is a plus, Xamarin Forms is a plus
8. Please don't spend more than 4 hours on this task, it's alright if you don't complete it

Please submit your solution into this repository.
Don't hesitate to contact us on any question you might have :)

Good luck!
