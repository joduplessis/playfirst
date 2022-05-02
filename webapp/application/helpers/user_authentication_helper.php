<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

if ( ! function_exists('check_authenticated_user')){
   function check_authenticated_user() {
       $CI = & get_instance();

   		if(!$CI->session->userdata('user')) {
   			redirect('', 'location');
   		}
   }
}
