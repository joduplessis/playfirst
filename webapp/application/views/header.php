<?php
defined('BASEPATH') OR exit('No direct script access allowed');
?>
<!DOCTYPE html>
<html lang="en">
<head>
	<meta charset="utf-8">
	<meta name="viewport" content="width=device-width,initial-scale=1">
	<meta name="author" content="name">
	<meta name="description" content="description here">
	<meta name="keywords" content="keywords,here">

	<title>Playfirst</title>

	<!-- Favourite icon -->
	<link rel="shortcut icon" href="<?php echo base_url();?>favicon.png" type="image/png" />

	<!-- Google fonts -->
	<link href="https://fonts.googleapis.com/css?family=Open+Sans:300,300i,400,400i,600,600i,700,700i,800,800i" rel="stylesheet">
	<link href="https://fonts.googleapis.com/css?family=Roboto+Slab:100,300,400,700" rel="stylesheet">
	<link href="https://fonts.googleapis.com/css?family=Ubuntu:300,300i,400,400i,500,500i,700,700i" rel="stylesheet">

	<!-- Jquery -->
	<script src="<?php echo base_url();?>bower_components/jquery/dist/jquery.min.js"></script>

	<!-- Tather -->
	<link href="<?php echo base_url();?>bower_components/tether/dist/css/tether.min.css" rel="stylesheet">
	<script src="<?php echo base_url();?>bower_components/tether/dist/js/tether.min.js"></script>

	<!-- Bootstrap -->
	<link href="<?php echo base_url();?>bower_components/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
	<script src="<?php echo base_url();?>bower_components/bootstrap/dist/js/bootstrap.min.js"></script>

	<!-- FontAwesome -->
	<link href="<?php echo base_url();?>bower_components/font-awesome/css/font-awesome.css" rel="stylesheet">

	<!-- Additional -->
	<script src="<?php echo base_url();?>custom_components/Chart.bundle.js"></script>

	<!-- Quill -->
	<link rel="stylesheet" href="<?php echo base_url();?>node_modules/react-quill/dist/quill.snow.css">
	<link rel="stylesheet" href="<?php echo base_url();?>node_modules/react-quill/dist/quill.bubble.css">
	<link rel="stylesheet" href="<?php echo base_url();?>node_modules/react-quill/dist/quill.core.css">

	<!-- Set the base URL -->
	<script>
		window.base_url = '<?php echo base_url();?>';
	</script>
</head>
<body>
	<div id="loading-screen" style=" position: fixed; top: 0px; left: 0px; width: 100%; height: 100%; z-index: 5000; background: #F6F6F7;"></div>
