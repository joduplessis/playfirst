<?php

  $admin_email = "jo@playfirst.co.za";
  $name = $_REQUEST['name'];
  $email = $_REQUEST['email'];
  $contact = $_REQUEST['contact'];
  $query = $name."\n".$email."\n".$contact."\n".$_REQUEST['query'];

  mail($admin_email, "Playfirst web query", $query, "From:" . $email);

  echo "<script>alert('Thank you for contacting us, we will get back to you as soon as possible.'); window.history.back();</script>";

?>
