
<div class="row navigation-header">
	<div class="col-1"></div>
	<div class="col-10">
		<h1>
			<?php echo $heading; ?>
			<ul>
				<li><button data-toggle="modal" data-target=".modal"><span class="fa fa-plus"></span> Add badge</button></li>
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
			<li><a href="#">Badges</a></li>
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
			<div class="col-12">
				<ul className="list-group">
					<?php foreach($badges as $badge) { ?>
						<li class="list-group-item">
							<div class="row" style="width: 100%;">
								<div class="col-1">
									<img src="<?php echo base_url()."image/thumbnail?image=".base_url()."".$badge->image; ?>" class="rounded-circle" width="50" height="50"/>
								</div>
								<div class="col-11">
									<a href="<?php echo base_url(); ?>badges/badge/<?php echo $badge->id; ?>" class="title"><?php echo $badge->title;?></a>
									<span class="description"><?php echo $badge->description;?></span>
									<span class="progress-background"><span class="active" style="width: <?php echo $badge->badge_count_percent; ?>%;"></span></span>
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





<div class="modal fade">
	<form action="<?php echo base_url();?>badges/badge_add" method="POST">
	  <div class="modal-dialog" role="document">
	    <div class="modal-content">
	      <div class="modal-header">
	        <h5 class="modal-title">New badge</h5>
	        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
	          <span aria-hidden="true">&times;</span>
	        </button>
	      </div>
	      <div class="modal-body">
	        <p>Choose a new name for your badge</p>
					<input type="text" placeholder="Title" name="title"/>
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
		$('.navigation a#badges').addClass('selected');
	});
</script>
</body>
</html>
