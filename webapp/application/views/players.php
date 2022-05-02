<div class="row navigation-header">
	<div class="col-1"></div>
	<div class="col-10">
		<h1>
			<?php echo $heading; ?>
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
			<li><a href="#">Players</a></li>
		</ul>
	</div>
	<div class="col-1"></div>
</div>

<div class="row pt-20">
	<div class="col-1"></div>
	<div class="col-10">
		<div class="alert alert-warning alert-dismissible fade show" role="alert">
		  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
		    <span aria-hidden="true">&times;</span>
		  </button>
		  <strong>Active Sync enabled</strong> Your players are linked to your network accounts.
		</div>
	</div>
	<div class="col-1"></div>
</div>

<div class="row grid">
	<div class="col-1"></div>
	<div class="col-10">
		<div class="row">
			<div class="col-12">
				<ul class="list-group">
					<?php foreach($players as $player) { ?>
						<li class="list-group-item">
							<div class="row" style="width: 100%;">
								<div class="col-1">
									<img src="<?php echo base_url();?>avatars/<?php echo $player->avatar ?>.png" class="rounded-circle" width="50" height="50"/>
								</div>
								<div class="col-10">
									<span class="title"><?php echo $player->fullname;?></span>
									<span class="description"><?php echo $player->playname;?></span>
									<span class="progress-background"><span class="active" style="width: <?php echo $player->logins_percent; ?>%;"></span></span>
								</div>
								<div class="col-1">
									<div class="mt-20">
										<span class="badge badge-default badge-pill"><?php echo $player->points ?></span>
									</div>
								</div>
							</div>
						</li>
					<?php } ?>
				</ul>
			</div>
		</div>
	</div>
	<div class="col-1"></div>
</div>

<script>
$(document).ready(function() {
	$('.navigation a#players').addClass('selected');
});
</script>
</body>
</html>
