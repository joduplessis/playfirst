<div class="row navigation-header">
	<div class="col-1"></div>
	<div class="col-10">
		<h1>
			Dashboard
			<ul>
				<li><button data-toggle="modal" data-target=".modal" onclick="#"><span class="fa fa-date"></span> Todays date is <strong><?php echo date("d M Y"); ?></button></li>
			</ul>
		</h1>
	</div>
	<div class="col-1"></div>
</div>

<div class="row pt-20">
	<div class="col-1"></div>
	<div class="col-10">
		<div class="alert alert-info alert-dismissible fade show" role="alert">
			<button type="button" class="close" data-dismiss="alert" aria-label="Close">
				<span aria-hidden="true">&times;</span>
			</button>
			<strong>Please note</strong> Playfirst is in alpha, please report any hickups to support.
		</div>
	</div>
	<div class="col-1"></div>
</div>

<div class="row pt-10">
	<div class="col-1"></div>
	<div class="col-10">
		<div style="border-radius: 3px; background: white; width: 100%;">
			<canvas id="canvas" style="height: 100px;" height="100"></canvas>
		</div>
	</div>
	<div class="col-1"></div>
</div>

<div class="row pt-30">
	<div class="col-1"></div>
	<div class="col-2">
		<div class="dash-block" style="background: #FF4971;">
			<div class="pre-head">Nodes completed</div>
			<div class="head"><?php echo $nodes_count; ?> <span class="note"><?php echo $nodes_count_percent; ?>% <span class="fa fa-pie-chart"></span></span></div>
			<div class="spacer"></div>
			<canvas id="canvas-one" style="width: 100%"; ></canvas>
		</div>
	</div>

	<div class="col-2">
		<div class="dash-block" style="background: #FF494C;">
			<div class="pre-head">Paths activated</div>
			<div class="head"><?php echo $paths_count; ?> <span class="note"><?php echo $paths_count_percent; ?>% <span class="fa fa-pie-chart"></span></span></div>
			<div class="spacer"></div>
			<canvas id="canvas-two" style="width: 100%"; ></canvas>
		</div>
	</div>

	<div class="col-2">
		<div class="dash-block" style="background: #FF775C;">
			<div class="pre-head">Logins</div>
			<div class="head"><?php echo $logins_count; ?> <span class="note"><?php echo $logins_count_percent; ?>% <span class="fa fa-pie-chart"></span></span></div>
			<div class="spacer"></div>
			<canvas id="canvas-three" style="width: 100%"; ></canvas>
		</div>
	</div>

	<div class="col-2">
		<div class="dash-block" style="background: #FFBE77;">
			<div class="pre-head">Badges collected</div>
			<div class="head"><?php echo $badges_count; ?> <span class="note"><?php echo $badges_count_percent; ?>% <span class="fa fa-pie-chart"></span></span></div>
			<div class="spacer"></div>
			<canvas id="canvas-four" style="width: 100%"; ></canvas>
		</div>
	</div>

	<div class="col-2">
		<div class="dash-block plain">
			<div class="pre-head"><?php echo $total_players; ?></div>
			<div class="head">Total number of players</div>
			<div class="spacer"></div>
		</div>
	</div>
	<div class="col-1"></div>
</div>

<script>
  new Chart(document.getElementById("canvas-one").getContext("2d"), {
      type: 'line',
      data: {
          labels: [<?php echo $months; ?>],
          datasets: [{
              label: "My First dataset",
              backgroundColor: 'rgba(255, 255, 255, 0.2)',
              borderColor: 'rgba(255, 255, 255, 0.7)',
              borderWidth: 2,
              fill: true,
              data: [<?php echo $nodes; ?>]
          }]
      },
      options: {
          elements: { point: { radius: 0 } },
          legend: false,
          responsive: true,
          tooltips: false,
          hover: false,
          scales: {
              xAxes: [{ display: false }],
              yAxes: [{ display: false }]
          }
      }
  });

  new Chart(document.getElementById("canvas-two").getContext("2d"), {
      type: 'line',
      data: {
          labels: [<?php echo $months; ?>],
          datasets: [{
              label: "My First dataset",
              backgroundColor: 'rgba(255, 255, 255, 0.2)',
              borderColor: 'rgba(255, 255, 255, 0.7)',
              borderWidth: 2,
              fill: true,
              data: [<?php echo $paths; ?>]
          }]
      },
      options: {
          elements: { point: { radius: 0 } },
          legend: false,
          responsive: true,
          tooltips: false,
          hover: false,
          scales: {
              xAxes: [{ display: false }],
              yAxes: [{ display: false }]
          }
      }
  });

	new Chart(document.getElementById("canvas-three").getContext("2d"), {
    type: 'line',
    data: {
        labels: [<?php echo $months; ?>],
        datasets: [{
            label: "My First dataset",
            backgroundColor: 'rgba(255, 255, 255, 0.2)',
            borderColor: 'rgba(255, 255, 255, 0.7)',
            borderWidth: 2,
            fill: true,
            data: [<?php echo $logins; ?>]
        }]
    },
    options: {
        elements: { point: { radius: 0 } },
        legend: false,
        responsive: true,
        tooltips: false,
        hover: false,
        scales: {
            xAxes: [{ display: false }],
            yAxes: [{ display: false }]
        }
    }
  });

  new Chart(document.getElementById("canvas-four").getContext("2d"), {
      type: 'line',
      data: {
          labels: [<?php echo $months; ?>],
          datasets: [{
              label: "My First dataset",
              backgroundColor: 'rgba(255, 255, 255, 0.2)',
              borderColor: 'rgba(255, 255, 255, 0.7)',
              borderWidth: 2,
              fill: true,
              data: [<?php echo $badges; ?>]
          }]
      },
      options: {
          elements: { point: { radius: 0 } },
          legend: false,
          responsive: true,
          tooltips: false,
          hover: false,
          scales: {
              xAxes: [{ display: false }],
              yAxes: [{ display: false }]
          }
      }
  });

	function convertHex(hex, opacity){
	    r = parseInt(hex.substring(0,2), 16);
	    g = parseInt(hex.substring(2,4), 16);
	    b = parseInt(hex.substring(4,6), 16);

	    result = 'rgba('+r+','+g+','+b+','+opacity+')';
	    return result;
	}

	var color = Chart.helpers.color;
	var chartColors = window.chartColors;

	new Chart(document.getElementById("canvas").getContext("2d"), {
		type: 'line',
		data: {
			labels: [<?php echo $months; ?>],
			datasets: [{
				label: "Nodes",
				backgroundColor: convertHex('EF007A', 0.15),
				borderColor: convertHex('EF007A', 0.1),
				data: [<?php echo $nodes; ?>],
			}, {
				label: "Paths",
				backgroundColor: convertHex('BE1285', 0.15),
				borderColor: convertHex('BE1285', 0.1),
				data: [<?php echo $paths; ?>],
			}, {
				label: "Logouts",
				backgroundColor: convertHex('7F0B81', 0.15),
				borderColor: convertHex('7F0B81', 0.1),
				data: [<?php echo $logouts; ?>],
			}, {
				label: "Logins",
				backgroundColor: convertHex('590460', 0.15),
				borderColor: convertHex('590460', 0.1),
				data: [<?php echo $logins; ?>],
			}]
		},
		options: {
			responsive: true,
			title:{
				display:true,
				text:"Player activity of the last few months"
			},
			tooltips: {
				mode: 'index',
			},
			hover: {
				mode: 'index'
			},
			scales: {
				xAxes: [{
					scaleLabel: {
						display: true,
						labelString: 'Month'
					}
				}],
				yAxes: [{
					stacked: true,
					scaleLabel: {
						display: true,
						labelString: 'Value'
					}
				}]
			}
		}
	});

	$(document).ready(function() {
		$('.navigation a#dashboard').addClass('selected');
	});
</script>
