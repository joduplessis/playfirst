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
			<li><a href="#">Assets</a></li>
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
		  <strong>Please note</strong> You are not able to load new assets, only edit existing ones.
		</div>
	</div>
	<div class="col-1"></div>
</div>
<?php if (isset($_GET['success'])) { ?>
	<div class="row">
		<div class="col-1"></div>
		<div class="col-10">
			<div class="alert alert-info alert-dismissible fade show" role="alert">
			  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
			    <span aria-hidden="true">&times;</span>
			  </button>
			  <strong>Success!</strong> Successfully updated
			</div>
		</div>
		<div class="col-1"></div>
	</div>
<?php } ?>

<div class="row grid">
	<div class="col-1"></div>
	<div class="col-10">
		<div class="row">
			<div class="col-12">
				<ul class="list-group">
					<?php foreach($assets as $asset) { ?>
						<li class="list-group-item">
							<div class="row" style="width: 100%;">
								<div class="col-1">
									<img src="<?php echo base_url();?>/<?php echo $asset->image;?>" width="50" height="50"/>
								</div>
								<div class="col-10">
									<a href="<?php echo base_url();?>assets/asset/<?php echo $asset->id;?>" class="title"><?php echo $asset->name;?></a>
									<span class="description"><?php echo $asset->prefab;?></span>
									<span class="progress-background"><span class="active" style="width: <?php echo $asset->placed_percent; ?>%;"></span></span>
								</div>
								<div class="col-1">
									<div class="mt-20">
										<span class="badge badge-default badge-pill"><?php echo $asset->placed; ?></span>
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
		$('.navigation a#assets').addClass('selected');
	});
</script>
</body>
</html>
