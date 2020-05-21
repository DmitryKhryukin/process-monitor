'use strict'

let eventSource = new EventSource('https://localhost:5001/api/processes');
let loadingDiv = document.getElementById("loading");
let processesInfoList = document.getElementById("processesInfo");

eventSource.addEventListener("open", function (e) {
  loadingDiv.hidden = false;
});

eventSource.addEventListener("message", function (e) {

  loadingDiv.hidden = true;
  processesInfoList.textContent = '';

  var data = JSON.parse(e.data);
  let processTable = generateProcessTable(data.Processes);
  
  let cpuLoad = data.CpuLoad.toFixed(2); 
  let cpuLoadInfo = document.createElement("div");
  cpuLoadInfo.innerHTML = `CPU Load: ${cpuLoad}%`;
  
  processesInfoList.appendChild(cpuLoadInfo);
  processesInfoList.appendChild(processTable);
});

eventSource.addEventListener("error", function (e) {
  console.log("error");
  console.log(e);

  processesInfoList.textContent = '';
  let errorInfo = document.createElement("div");
  errorInfo.innerHTML = "Server error";
  processesInfoList.appendChild(errorInfo);
});

function generateProcessTable(processes) {

  if (processes.length > 0) {

    let tbl = document.createElement("table");
    let tblBody = document.createElement("tbody");

    let header = document.createElement("thead");
    let headerRow = document.createElement("tr");
    
    
    for(const property in processes[0]){
      let cell = document.createElement("th");
      let cellText = document.createTextNode(property);
      cell.appendChild(cellText);
      headerRow.appendChild(cell);
    }

    header.appendChild(headerRow);
    tblBody.appendChild(headerRow);

    for (var i = 0; i < processes.length; i++) {

      var row = document.createElement("tr");

      for (const property in processes[i]) {

        let cell = document.createElement("td");
        let cellText = document.createTextNode(processes[i][property]);
        cell.appendChild(cellText);
        row.appendChild(cell);
      }

      tblBody.appendChild(row);
    }

    // put the <tbody> in the <table>
    tbl.appendChild(tblBody);

    // sets the border attribute of tbl to 2;
    tbl.setAttribute("border", "2");

    return tbl;
  } else {
    let noInfoMessage = document.createElement("div");
    noInfoMessage.innerHTML = "No information about processes";
    return noInfoMessage;
  }
}