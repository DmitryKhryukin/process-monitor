'use strict'


let eventSource = new EventSource('https://localhost:5001/api/processes');

eventSource.addEventListener("open", function(e) { 
    console.log(e.data);
    console.log(JSON.stringify(e));
    
  });

eventSource.addEventListener("message", function(e) {
    console.log("MESSAGE");
    console.log(JSON.stringify(e));
    document.getElementById("processesInfo").innerHTML = JSON.stringify(e.data);
    console.log(e.data);
  });

eventSource.addEventListener("error", function(e) {
    console.log("error");
    console.log(JSON.stringify(e));
    console.log(e.data);
  });