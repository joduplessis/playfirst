
<div class="row navigation-header">
	<div class="col-1"></div>
	<div class="col-10">
		<h1>
			<?php echo $heading; ?>
			<ul>
				<li><button data-toggle="modal" data-target=".modal"><span class="fa fa-plus"></span> Add player learning path</button></li>
				<li><button data-toggle="modal" data-target=".modal.scorm"><span class="fa fa-upload"></span> Import SCORM content</button></li>
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
			<li><a href="#">Paths</a></li>
		</ul>
	</div>
	<div class="col-1"></div>
</div>

<?php if (isset($_GET['success'])) { ?>
	<div class="row pt-20">
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
	<?php foreach($paths as $path) { ?>
	<div class="col-2">
		<div class="inner">
			<h1><a href="<?php echo base_url();?>paths/path/<?php echo $path->id;?>"><?php echo $path->title;?></a></h1>
			<h2><?php echo $path->description;?></h2>
			<p class="images">
				<img class="rounded-circle" src="<?php echo base_url();?>avatars/<?php echo $path->user_image; ?>.png" width="30" height="30">
			</p>
			<span class="progress-background"><span class="active" style="width: <?php echo $path->activations_percent; ?>%;"></span></span>
		</div>
	</div>
	<?php } ?>
	<div class="col-1"></div>
</div>

<div class="modal fade">
	<form action="<?php echo base_url();?>paths/add_path" method="POST">
	  <div class="modal-dialog" role="document">
	    <div class="modal-content">
	      <div class="modal-header">
	        <h5 class="modal-title">New learning path</h5>
	        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
	          <span aria-hidden="true">&times;</span>
	        </button>
	      </div>
	      <div class="modal-body">
	        <p>Choose a new name for your learning path</p>
					<input type="text" placeholder="New path" name="title"/>
	      </div>
	      <div class="modal-footer">
	        <button type="submit" class="btn btn-primary">Add</button>
	        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
	      </div>
	    </div>
	  </div>
	</form>
</div>

<div class="modal scorm fade">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Scorm</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <p>Coming soon</p>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>

















<script>
	$(document).ready(function() {
		$('.navigation a#paths').addClass('selected');
	});
</script>
