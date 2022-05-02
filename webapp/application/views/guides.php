
<div class="row navigation-header">
	<div class="col-1"></div>
	<div class="col-10">
		<h1>
			<?php echo $heading; ?>
			<ul>
				<li><button data-toggle="modal" data-target=".modal"><span class="fa fa-plus"></span> Add guide</button></li>
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
			<li><a href="#">Guides</a></li>
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
					<?php foreach($guides as $guide) { ?>
						<li class="list-group-item">
							<div class="row" style="width: 100%;">
								<div class="col-12">
									<a href="<?php echo base_url(); ?>guides/guide/<?php echo $guide->id; ?>" class="title"><?php echo $guide->text;?></a>
									</br>
									<span class="description"><?php echo $guide->node_title; ?></span>
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
	<form action="<?php echo base_url();?>guides/guide_add" method="POST">
	  <div class="modal-dialog" role="document">
	    <div class="modal-content">
	      <div class="modal-header">
	        <h5 class="modal-title">New guide</h5>
	        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
	          <span aria-hidden="true">&times;</span>
	        </button>
	      </div>
	      <div class="modal-body">
	        <p>Text for the guide</p>
					<input type="text" placeholder="New guide" name="text"/>
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
		$('.navigation a#guides').addClass('selected');
	});
</script>
</body>
</html>
