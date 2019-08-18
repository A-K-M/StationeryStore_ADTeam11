
function transformIntoJsList(documentName, childNodeNames) {
    let counter = $(`#${documentName} > input`).length;

    let intList = [];

    for (let i = 0; i < counter; i++) {
        let intVal = $(`#${childNodeNames}-${i}`).val();
        intList.push(intVal);
    }

    return intList;
}


$(document).ready(function () {

    'use strict';

    //var paper = Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(Viewdata["paper"]));
        //var test = $.csv.toArray("D:\AD Workspace\MLtesting\ReorderChart.csv");
        //var test = ViewData["paper"];

    var reorder_paper = transformIntoJsList("paper-list", "paper");
    var reorder_pen = transformIntoJsList("pen-list", "pen");
    var reorder_stapler = transformIntoJsList("stapler-list", "stapler");

    var request_paper = transformIntoJsList("request-paper-list", "request-paper");
    var request_pen = transformIntoJsList("request-pen-list", "request-pen");
    var request_stapler = transformIntoJsList("request-stapler-list", "request-stapler");

    //var request_paper = [65, 59, 80, 81, 56];//transformIntoJsList("paper-list", "paper"); //[65, 59, 80, 81, 56];
    //var request_pen = [30, 81, 56, 55, 40];//transformIntoJsList("pen-list", "pen");//[30, 81, 56, 55, 40];
    //var request_stapler = [15, 9, 30, 21, 6];//transformIntoJsList("stapler-list", "stapler");//[15, 9, 30, 21, 6];
    
    var brandPrimary = 'rgba(3, 244, 252, 1)';

    var LINECHARTEXMPLE = $('#lineChartExample'),
        BARCHARTEXMPLE = $('#barChartExample');

    var lineChartExample = new Chart(LINECHARTEXMPLE, {
        type: 'line',
        data: {
            labels: ["January", "February", "March", "April", "May", "June", "July","August","September","October","November","December"],
            datasets: [
                {
                    label: "Paper",
                    fill: true,
                    lineTension: 0.3,
                    backgroundColor: "rgba(3, 244, 252, 0.38)",
                    borderColor: brandPrimary,
                    borderCapStyle: 'butt',
                    borderDash: [],
                    borderDashOffset: 0.0,
                    borderJoinStyle: 'miter',
                    borderWidth: 1,
                    pointBorderColor: brandPrimary,
                    pointBackgroundColor: "#fff",
                    pointBorderWidth: 1,
                    pointHoverRadius: 5,
                    pointHoverBackgroundColor: brandPrimary,
                    pointHoverBorderColor: "rgba(3, 244, 252,1)",
                    pointHoverBorderWidth: 2,
                    pointRadius: 1,
                    pointHitRadius: 10,
                    data: reorder_paper,
                    //data: [50, 20, 40, 31, 32, 22, 10],
                    spanGaps: false
                },
                {
                    label: "Pencil",
                    fill: true,
                    lineTension: 0.3,
                    backgroundColor: "rgba(75,192,192,0.4)",
                    borderColor: "rgba(75,192,192,1)",
                    borderCapStyle: 'butt',
                    borderDash: [],
                    borderDashOffset: 0.0,
                    borderJoinStyle: 'miter',
                    borderWidth: 1,
                    pointBorderColor: "rgba(75,192,192,1)",
                    pointBackgroundColor: "#fff",
                    pointBorderWidth: 1,
                    pointHoverRadius: 5,
                    pointHoverBackgroundColor: "rgba(75,192,192,1)",
                    pointHoverBorderColor: "rgba(220,220,220,1)",
                    pointHoverBorderWidth: 2,
                    pointRadius: 1,
                    pointHitRadius: 10,
                    data: reorder_pen,
                    //data: [65, 59, 30, 81, 56, 55, 40],
                    spanGaps: false
                },
                {
                    label: "Stapler",
                    fill: true,
                    lineTension: 0.3,
                    backgroundColor: "rgba(51, 179, 90,0.4)",
                    borderColor: "rgba(51, 179, 90,1)",
                    borderCapStyle: 'butt',
                    borderDash: [],
                    borderDashOffset: 0.0,
                    borderJoinStyle: 'miter',
                    borderWidth: 1,
                    pointBorderColor: "rgba(51, 179, 90,1)",
                    pointBackgroundColor: "#fff",
                    pointBorderWidth: 1,
                    pointHoverRadius: 5,
                    pointHoverBackgroundColor: "rgba(51, 179, 90,1)",
                    pointHoverBorderColor: "rgba(51, 179, 90,1)",
                    pointHoverBorderWidth: 2,
                    pointRadius: 1,
                    pointHitRadius: 10,
                    data: reorder_stapler,
                    //data: [15, 9, 30, 21, 6, 14, 18],
                    spanGaps: false
                }
            ]
        }
    });




    var barChartExample = new Chart(BARCHARTEXMPLE, {
        type: 'bar',
        data: {
            labels: ["COMM", "CPSC", "ENGL", "REGR", "ZOOL"],
            datasets: [
                {
                    label: "Paper",
                    backgroundColor: [
                        'rgba(51, 179, 90, 0.6)',
                        'rgba(51, 179, 90, 0.6)',
                        'rgba(51, 179, 90, 0.6)',
                        'rgba(51, 179, 90, 0.6)',
                        'rgba(51, 179, 90, 0.6)',
                        'rgba(51, 179, 90, 0.6)',
                        'rgba(51, 179, 90, 0.6)',
                        'rgba(51, 179, 90, 0.6)',
                        'rgba(51, 179, 90, 0.6)',
                        'rgba(51, 179, 90, 0.6)'
                    ],
                    borderColor: [
                        'rgba(51, 179, 90, 1)',
                        'rgba(51, 179, 90, 1)',
                        'rgba(51, 179, 90, 1)',
                        'rgba(51, 179, 90, 1)',
                        'rgba(51, 179, 90, 1)',
                        'rgba(51, 179, 90, 1)',
                        'rgba(51, 179, 90, 1)',
                        'rgba(51, 179, 90, 1)',
                        'rgba(51, 179, 90, 1)',
                        'rgba(51, 179, 90, 1)'
                    ],
                    borderWidth: 1,
                    data: request_paper,
                },
                {
                    label: "Pen",
                    backgroundColor: [
                        'rgba(203, 203, 203, 0.6)',
                        'rgba(203, 203, 203, 0.6)',
                        'rgba(203, 203, 203, 0.6)',
                        'rgba(203, 203, 203, 0.6)',
                        'rgba(203, 203, 203, 0.6)',
                        'rgba(203, 203, 203, 0.6)',
                        'rgba(203, 203, 203, 0.6)',
                        'rgba(203, 203, 203, 0.6)',
                        'rgba(203, 203, 203, 0.6)',
                        'rgba(203, 203, 203, 0.6)'
                    ],
                    borderColor: [
                        'rgba(203, 203, 203, 1)',
                        'rgba(203, 203, 203, 1)',
                        'rgba(203, 203, 203, 1)',
                        'rgba(203, 203, 203, 1)',
                        'rgba(203, 203, 203, 1)',
                        'rgba(203, 203, 203, 1)',
                        'rgba(203, 203, 203, 1)',
                        'rgba(203, 203, 203, 1)',
                        'rgba(203, 203, 203, 1)',
                        'rgba(203, 203, 203, 1)'
                    ],
                    borderWidth: 1,
                    data: request_pen,
                },
                {
                    label: "Stapler",
                    backgroundColor: [
                        'rgba(229, 186, 255, 0.6)',
                        'rgba(229, 186, 255, 0.6)',
                        'rgba(229, 186, 255, 0.6)',
                        'rgba(229, 186, 255, 0.6)',
                        'rgba(229, 186, 255, 0.6)',
                        'rgba(229, 186, 255, 0.6)',
                        'rgba(229, 186, 255, 0.6)',
                        'rgba(229, 186, 255, 0.6)',
                        'rgba(229, 186, 255, 0.6)',
                        'rgba(229, 186, 255, 0.6)'
                    ],
                    borderColor: [
                        'rgba(229, 186, 255, 1)',
                        'rgba(229, 186, 255, 1)',
                        'rgba(229, 186, 255, 1)',
                        'rgba(229, 186, 255, 1)',
                        'rgba(229, 186, 255, 1)',
                        'rgba(229, 186, 255, 1)',
                        'rgba(229, 186, 255, 1)',
                        'rgba(229, 186, 255, 1)',
                        'rgba(229, 186, 255, 1)',
                        'rgba(229, 186, 255, 1)'
                    ],
                    borderWidth: 1,
                    data: request_stapler,
                }
            ]
        }
    });

});


