<div class="row navigation-header">
	<div class="col-1"></div>
	<div class="col-10">
		<h1>
			<?php echo $heading; ?>
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
			<li><a href="javascript:window.history.back();">Paths</a></li>
			<li>/</li>
			<li><a href="#"><?php echo $path->title; ?></a></li>
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
	<div class="col-10">
		<div class="row">
			<div class="col-4">
				<div class="inner pt-10 pb-10 pl-10 pr-10">
					<h1>Path</h1>
					<form action="<?php echo base_url();?>paths/update_path" method="POST">
						<input type="hidden" name="id" value="<?php echo $path->id; ?>" />
						<input type="hidden" name="user" value="<?php echo $user; ?>" />

					  <div class="form-group">
					    <label for="exampleInputEmail1">Title</label>
					    <input type="text" class="form-control form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Title" value="<?php echo $path->title; ?>" name="title">
					  </div>
						<div class="form-group">
					    <label for="exampleTextarea">Description</label>
					    <textarea class="form-control" id="exampleTextarea" rows="10" name="description"><?php echo $path->description; ?></textarea>
					  </div>
					  <div class="form-group">
					    <label for="exampleSelect1">Pre-requisite path</label>
							<select name="requirement" class="form-control" id="exampleSelect1">
								<option value="0">Please select a path</option>
								<?php foreach($paths as $p) { ?>
									<?php if ($p->id==$path->requirement) { ?>
										<option selected value="<?php echo $p->id ?>"><?php echo $p->title ?></option>
									<?php } else { ?>
										<option value="<?php echo $p->id ?>"><?php echo $p->title ?></option>
									<?php } ?>
								<?php } ?>
							</select>
					  </div>
					  <button type="submit" class="btn btn-primary">Save</button>
					</form>
				</div>
				<a href="<?php echo base_url();?>paths/delete_path/<?php echo $path->id;?>" class="footer" id="<?php echo $path->id ?>" onclick="return confirm('Are you sure you want to delete this path?');"><span class="fa fa-trash"></span> Delete</a>
			</div>
			<div class="col-8">
				<div class="inner pt-10 pb-10 pl-10 pr-10">
					<h1>Nodes</h1>
					<label>List of nodes attached to this path</label>
					<ul class="list-group justify-content-between">
						<?php foreach($nodes as $n) { ?>
					  <li class="list-group-item justify-content-between">
							<a href="<?php echo base_url();?>paths/path_node/<?php echo $n->type; ?>/<?php echo $n->id; ?>">
								<span class="type"><?php echo $n->type; ?></span>
								<?php echo substr($n->title, 0, 35)." ..."; ?>
								<span class="completions"><span class="active" style="width: <?php echo $n->completions_percent; ?>%;"></span></span>
							</a>
							<span class="badge badge-default badge-pill"><?php echo $n->completions; ?> completed</span>
					  </li>
						<?php } ?>
					</ul>

				</div>
				<a href="" class="footer" id="<?php echo $path->id ?>" data-toggle="modal" data-target=".modal"><span class="fa fa-plus"></span> Add new node</a>
			</div>
		</div>
	</div>
</div>




<div class="modal fade">
	<form action="<?php echo base_url();?>paths/add_node" method="POST">
		<input type="hidden" name="path" value="<?php echo $path->id ?>">

	  <div class="modal-dialog" role="document">
	    <div class="modal-content">
	      <div class="modal-header">
	        <h5 class="modal-title">New node</h5>
	        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
	          <span aria-hidden="true">&times;</span>
	        </button>
	      </div>
	      <div class="modal-body">
	        <p>Choose a new name for your node</p>
					<div class="form-group">
						<label for="exampleInputEmail1">Title</label>
						<input type="text" class="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Title" name="title">

						<label for="exampleSelect1">Pre-requisite path</label>
						<select name="type" class="form-control" id="exampleSelect1">
							<option value="mixed">Mixed content</option>
							<option value="choices">Multiple choice</option>
							<option value="video">Video</option>
							<option value="images">Image slideshow</option>
							<option value="wind">Wind (mini game)</option>
						</select>
					</div>
	      </div>
	      <div class="modal-footer">
	        <button type="submit" class="btn btn-primary">Add</button>
	        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
	      </div>
	    </div>
	  </div>
	</form>
</div>








<script>
	$(document).ready(function() {
		$('.navigation a#paths').addClass('selected');
	});
</script>
