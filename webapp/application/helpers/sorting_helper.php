<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

if ( ! function_exists('compare_points')){
  function compare_points($a, $b) {
    if($a->points == $b->points) {
      return 0;
    }
    return ($a->points > $b->points) ? -1 : 1;
  }
}
