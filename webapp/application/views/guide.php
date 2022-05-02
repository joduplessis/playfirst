
<div class="row navigation-header">
	<div class="col-1"></div>
	<div class="col-10">
		<h1>
			<?php echo $title; ?>
			<ul>
				<li><button data-toggle="modal" data-target=".modal" onclick="window.history.back();"><span class="fa fa-chevron-left"></span> Go back</button></li>
			</ul>
		</h1>
	</div>
	<div class="col-1"></div>
</div>

<div class="row">
	<div class="col-1"></div>
	<div class="col-10">
		<ul class="breadcrumbs">
			<li><a href="<?php echo base_url();?>">Home</a></li>
			<li>/</li>
			<li><a href="javascript:window.history.back();">Guide</a></li>
			<li>/</li>
			<li><a href="#"><?php echo $title; ?></a></li>
		</ul>
	</div>
	<div class="col-1"></div>
</div>

<!-- React components here -->

<script>
	$(document).ready(function() {
		$('.navigation a#guides').addClass('selected');
	});
</script>
