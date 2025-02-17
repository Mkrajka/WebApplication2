
// Code for the Bar Graph
// Explanatory data for the bar graph
var daysOfWeek = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

// Function to rotate array to position current day at the end
function rotateArray(arr, count) {
    return arr.slice(count, arr.length).concat(arr.slice(0, count));
}

// Get the current day index (0 for Sunday, 1 for Monday, etc.)
var currentDayIndex = new Date().getDay() - 6;

// Rotate the daysOfWeek array to position the current day at the end
var rotatedDays = rotateArray(daysOfWeek, currentDayIndex);

var entryLogsPerDayData = @Html.Raw(entryLogsPerDayJson);



var data = {
    labels: rotatedDays,
    datasets: [{
        label: 'Number of Entries',
        data: entryLogsPerDayData,
        // data: [4, 8, 12, 6, 10, 18, 7], // Replace with your actual data data: entryCounts,
        backgroundColor: '#3DD9EB',
        borderColor: '#3DD9EB',
        borderWidth: 1,
        borderRadius: 7,
        borderSkipped: false,
    }]
};


// Bar graph options
var options = {
    scales: {
        x: {
            type: 'category',
            position: 'bottom',
            grid: {
                display: false,
            },
            ticks: {
                color: 'black'
            },
            border: {
                display: false
            },
        },
        y: {
            beginAtZero: true,
            ticks: {
                display: false
            },
            grid: {
                display: false,
            },
            border: {
                display: false
            },
        },
    },
    plugins: {
        legend: {
            display: false // Set to false to hide legend
        }
    },
    indexAxis: 'x', // Set the axis for barPercentage and categoryPercentage
    barPercentage: 0.9, // Adjust the bar width (0.5 means 50% of the available space)
    // categoryPercentage: 0.01 // Adjust the space between bars (0.7 means 70% of the available space)
};

var ctx = document.getElementById('entryLogsChart').getContext('2d');
var entryLogsChart = new Chart(ctx, {
    type: 'bar',
    data: data,
    options: options
});
