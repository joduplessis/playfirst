<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

if ( ! function_exists('time_elapsed_string')){
  function time_elapsed_string($datetime, $full = false) {
    date_default_timezone_set('Africa/Johannesburg');

	  $now = new DateTime;
	  $ago = new DateTime($datetime);
	  $diff = $now->diff($ago);

	  $diff->w = floor($diff->d / 7);
	  $diff->d -= $diff->w * 7;

	  $string = array(
	    'y' => 'yr',
	    'm' => 'mo',
	    'w' => 'w',
	    'd' => 'd',
	    'h' => 'h',
	    'i' => 'm',
	    's' => 's',
	  );
	  foreach ($string as $k => &$v) {
	    if ($diff->$k) {
	      $v = $diff->$k . ' ' . $v . ($diff->$k > 1 ? 's' : '');
	    } else {
	      unset($string[$k]);
	    }
	  }

	  if (!$full) $string = array_slice($string, 0, 1);
	  return $string ? implode(', ', $string) . ' ago' : 'just now';
	}
}
