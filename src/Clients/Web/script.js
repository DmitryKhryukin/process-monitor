'use strict'

let eventSource = new EventSource('https://localhost:5001/api/processes');
let loadingDiv = document.getElementById("loading");
let processesInfoList = document.getElementById("processesInfo");

eventSource.addEventListener("open", function(e) { 
    loadingDiv.hidden = false;
});

eventSource.addEventListener("message", function(e) {
    
    loadingDiv.hidden = true;
    processesInfoList.textContent = '';

    var data = JSON.parse(e.data);
    data.forEach(function(item, index) {
        var elem = document.createElement("li");
        elem.innerHTML = "<div>"+item.ProcessName+"</div>";
        processesInfoList.appendChild(elem);
    });
  });

eventSource.addEventListener("error", function(e) {
    console.log("error");
    console.log(JSON.stringify(e));
    console.log(e.data);
  });