var monthsOfYear = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

function rotateArray(arr, count) {
    return arr.slice(count, arr.length).concat(arr.slice(0, count));
}

var currentMonthIndex = new Date().getMonth() + 1;
var rotatedMonths = rotateArray(monthsOfYear, currentMonthIndex);

var gradientJoining = ctx.createLinearGradient(0, 0, 0, 300);
gradientJoining.addColorStop(0, 'rgba(48, 77, 112, 1)'); // Starting color (same as borderColor)
gradientJoining.addColorStop(1, 'rgba(255, 216, 3, 0)'); // Ending color

var gradientLeaving = ctx.createLinearGradient(0, 0, 0, 300);
gradientLeaving.addColorStop(0, 'rgba(65, 184, 213, 1)'); // Starting color (same as borderColor)
gradientLeaving.addColorStop(1, 'rgba(82, 82, 82, 0)'); // Ending color

var data = {
    labels: rotatedMonths,
    datasets: [
        {
            label: 'Joining Members',
            data: [@string.Join(",", @Model.MembersJoinedPerMonth.Values.Reverse().Select(entry => entry.Count))],
            borderColor: '#304D70',
            backgroundColor: gradientJoining,
            borderWidth: 1,
            pointRadius: 0,
            tension: 0.4,
            hoverRadius: 8,
            hitRadius: 10,
            fill: true,
        },
        {
            label: 'Leaving Members',
            data: [@string.Join(",", @Model.MembersLeftPerMonth.Values.Reverse().Select(entry => entry.Count))],
            borderColor: '#41B8D5',
            backgroundColor: gradientLeaving,
            borderWidth: 1,
            pointRadius: 0,
            tension: 0.4,
            hoverRadius: 8,
            hitRadius: 10,
            fill: true,
        }
    ]
};

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
                display: false, // Set display to false to remove y-axis grid lines
            },
            border: {
                display: false
            },
        },
    },
    plugins: {
        legend: {
            display: false
        },
        tooltip: {
            enabled: true,
            intersect: false,
        }
    },
    interaction: {
        mode: 'nearest',
    },
};
