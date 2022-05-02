
<div class="row navigation-header">
	<div class="col-1"></div>
	<div class="col-10">
		<h1>
			<?php echo $asset[0]->name; ?>
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
			<li><a href="javascript:window.history.back();">Asset</a></li>
			<li>/</li>
			<li><a href="#"><?php echo $asset[0]->name; ?></a></li>
		</ul>
	</div>
	<div class="col-1"></div>
</div>

<div class="row grid">
	<div class="col-1"></div>
	<div class="col-10">
		<div class="row">
			<div class="col-8">
				<div class="inner pt-10 pb-10 pl-10 pr-10">
					<h1>Asset</h1>
					<form action="<?php echo base_url();?>assets/update" method="POST">
						<input type="hidden" name="id" value="<?php echo $asset[0]->id; ?>" />
						<input type="hidden" name="image" value="<?php echo $asset[0]->image; ?>"/>
						<input type="hidden" name="prefab" value="<?php echo $asset[0]->prefab; ?>"/>
						<input type="hidden" name="name" value="<?php echo $asset[0]->name; ?>"/>

					  <div class="form-group">
					    <label for="exampleInputEmail1">Point to build</label>
					    <input type="text" class="form-control form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Title" name="build_points" value="<?php echo $asset[0]->build_points; ?>">
					  </div>

						<div class="form-group">
							<label for="exampleInputEmail1">Build time in seconds</label>
							<input type="text" class="form-control form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Title" name="build_time" value="<?php echo $asset[0]->build_time; ?>">
						</div>

						<div class="form-group">
							<label for="exampleInputEmail1">Point this unit generates</label>
							<input type="text" class="form-control form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Title" name="generate_points" value="<?php echo $asset[0]->generate_points; ?>">
						</div>

						<div class="form-group">
							<label for="exampleInputEmail1">Time to generate points</label>
							<input type="text" class="form-control form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Title" name="generate_time" value="<?php echo $asset[0]->generate_time; ?>">
						</div>

					  <button type="submit" class="btn btn-primary">Save</button>
					</form>
				</div>
			</div>
			<div class="col-4">
				<div class="inner pt-10 pb-10 pl-10 pr-10">
					<h1 class="pb-20"><?php echo $asset[0]->name; ?></h1>
					<img src="<?php echo base_url();?><?php echo $asset[0]->image; ?>" width="90%"/>
				</div>
			</div>
		</div>
	</div>
</div>


<script>
	$(document).ready(function() {
		$('.navigation a#assets').addClass('selected');
	});
</script>
