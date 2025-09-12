# Quality Assurance Tests
The purpose of this document is to describe the process of manual testing. This is a work in progress, and may be updated as the project progresses.

## Initializing Project
Information on how to initialize the project can be found [here](Readme.md).
After successful initialization, the App should be running on [localhost:3000](http://localhost:3000). Ensure the Api is running on [localhost:3001](http://localhost:5010) by checking [/health](http://localhost:5010/health)

## Testing
### Basic Connectivity Tests
1. Frontend loads correctly at http://localhost:3000
   * Should see "Shape Generator" title
   * Input field and "Generate Shape" button visible
   * No console errors in browser dev tools
2. Api is running at http://localhost:5010
   * Should see "Healthy" status @[/health](http://localhost:5010/health)
   * No console errors in browser dev tools
3. Api is reachable from frontend
   * In browser, enter a valid shape definition in the input field
   * Click "Generate Shape" button
   * Should see a response from the Api @[/api/shape](http://localhost:5010/api/shape)
   * In dev tools, should see no errors and Network tab should show a 200 response
   * No console errors in browser dev tools

### Shape Generation Tests
Example command inputs can be found underneath the input field.
1. Circle shape
   * Enter "Draw a circle with a radius of 100" in input field
   * Click "Generate Shape" button
   * Should see a circle shape in the canvas
2. Square shape
   * Enter "Draw a Square with a side length of 200" in input field
   * Click "Generate Shape" button
   * Should see a rectangle shape in the canvas

### Error Handling Tests
6. Invalid command input
   * Enter "this is not a valid command" in input field
   * Click "Generate Shape" button
   * Should see an error message in the console
   * Should see a "Shape not generated" message in the canvas
7. Invalid shape definition
   * Enter "Draw a star with a radius of 100" in input field
   * Click "Generate Shape" button
   * Should see an error message in the console
   * Should see a "Shape not generated" message in the canvas
8. Negative value shape
    * Enter "Draw a circle with a radius of -100" in input field
    * Click "Generate Shape" button
    * Should see an error message in the console
    * Should see a "Shape not generated" message in the canvas
