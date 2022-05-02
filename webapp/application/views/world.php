
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
			<li><a href="javascript:window.history.back();">World</a></li>
			<li>/</li>
			<li><a href="#"><?php echo $title; ?></a></li>
		</ul>
	</div>
	<div class="col-1"></div>
</div>

<div class="row grid">
	<div class="col-1"></div>
	<div class="col-10">
		<div class="row">
			<div class="col-12">
				<div class="inner pt-10 pb-10 pl-10 pr-10">
					<h1>World</h1>
					<form action="<?php echo base_url();?>worlds/world_update" method="POST">
						<input type="hidden" name="id" value="<?php echo $world->id; ?>" />

					  <div class="form-group">
					    <label for="exampleInputEmail1">Title</label>
					    <input name="title" type="text" class="form-control form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Title" value="<?php echo $world->title; ?>">
					  </div>
						<div class="form-group">
					    <label for="exampleTextarea">Player starting world</label>

					    <input type="checkbox" class="form-control" id="exampleTextarea" rows="10" name="start" <?php if ($world->start) { ?> checked <?php } ?>/>
					  </div>

					  <button type="submit" class="btn btn-primary">Save</button>
					</form>
				</div>
				<a href="<?php echo base_url();?>worlds/world_delete/<?php echo $world->id;?>" class="footer" id="<?php echo $world->id ?>" onclick="return confirm('Are you sure you want to delete this world?');"><span class="fa fa-trash"></span> Delete</a>
			</div>
		</div>
	</div>
</div>











<script>
	$(document).ready(function() {
		$('.navigation a#worlds').addClass('selected');
	});
</script>
