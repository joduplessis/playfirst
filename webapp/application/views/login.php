
<div class="jumbotron jumbotron-fluid login">
	<div class="container">
		<div class="col-6">
			<form action="<?php echo base_url();?>management/login/do" method="POST">
				<a href="#" class="logo"><img src="<?php echo base_url();?>images/logo.png" /> <span style="text-transform: lowercase;font-weight: 400;"></span></a>
				<h1>Playfirst is a learning platform and is in alpha right now, to get login credentials please <a href="">contact us</a>.</h1>
				<input type="text" placeholder="email" name="email" value=""/><br/>
				<input type="password" placeholder="password" name="password" value=""/><br/>
				<button type="submit">login</button>
			</form>
		</div>
	</div>
</div>
