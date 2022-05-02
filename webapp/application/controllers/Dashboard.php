<?php
header('Access-Control-Allow-Origin: *');
defined('BASEPATH') OR exit('No direct script access allowed');
date_default_timezone_set('Africa/Johannesburg');

class Dashboard extends CI_Controller {

	public function __construct() {
		parent::__construct();

		$this->load->library('session');
	}

	public function index() {
    $this->load->model('Activity_model');
		$this->load->helper('user_authentication');
		$this->load->model('User_model');

		// Called from our helpers
		check_authenticated_user();

		$months_array = [];

		$node_values_array = [];
		$path_values_array = [];
		$login_values_array = [];
		$logout_values_array = [];
		$badge_values_array = [];

		$node_count = 0;
		$path_count = 0;
		$login_count = 0;
		$logout_count = 0;
		$badge_count = 0;

		$man_badges = [1,3,5];
		$man_paths = [1];
		$man_nodes = [2,10,21,22];
		$man_worlds = [1,2,5];
		$man_mwo = [179,180,181,182,183,184,185,186,187,188,189,190,191,192,193,194,195];




		/*

		Populate fake data

		["badge", "node_complete", "node_open", "path_activate", "world_enter", "login", "like"]

		for ($a=0; $a<200; $a++) {
			$type = "logout";
			$arr = $man_paths;
			$user = rand(1, 8);
			$created_at = rand(1475931293, 1486558506);

			$target = $arr[rand(0, count($arr)-1)];
			$this->Activity_model->log_manual($user, $type, $target, $created_at);
		}
		*/

		for ($i = 4; $i >= 0; $i--) {
			$month = date('M', strtotime("-$i month", strtotime('first day this month')));
			$query_date = date('M d y', strtotime("-$i month", strtotime('first day this month')));
			$start_date = date('Y-m-01', strtotime($query_date));
			$end_date = date('Y-m-t', strtotime($query_date));

			array_push($months_array, "'".$month."'");

			$path_count_for_this_month = $this->Activity_model->get_type_count_by_dates("path_activate", strtotime($start_date), strtotime($end_date));
			$node_count_for_this_month = $this->Activity_model->get_type_count_by_dates("node_open", strtotime($start_date), strtotime($end_date));
			$login_count_for_this_month = $this->Activity_model->get_type_count_by_dates("login", strtotime($start_date), strtotime($end_date));
			$logout_count_for_this_month = $this->Activity_model->get_type_count_by_dates("logout", strtotime($start_date), strtotime($end_date));
			$badge_count_for_this_month = $this->Activity_model->get_type_count_by_dates("badge", strtotime($start_date), strtotime($end_date));

			$node_count += $node_count_for_this_month;
			$path_count += $path_count_for_this_month;
			$login_count += $login_count_for_this_month;
			$logout_count += $logout_count_for_this_month;
			$badge_count += $badge_count_for_this_month;

			array_push($node_values_array, $node_count_for_this_month);
			array_push($path_values_array, $path_count_for_this_month);
			array_push($login_values_array, $login_count_for_this_month);
			array_push($logout_values_array, $logout_count_for_this_month);
			array_push($badge_values_array, $badge_count_for_this_month);
		}

		$data["playname"] = $this->session->user[0]->playname;
		$data["heading"] = "Dashboard";
		$data["months"] = join(',', $months_array);

		$data["nodes"] = join(',', $node_values_array);
		$data["paths"] = join(',', $path_values_array);
		$data["logins"] = join(',', $login_values_array);
		$data["logouts"] = join(',', $logout_values_array);
		$data["badges"] = join(',', $badge_values_array);

		$data["nodes_count"] = $node_count;
		$data["paths_count"] = $path_count;
		$data["logins_count"] = $login_count;
		$data["logouts_count"] = $logout_count;
		$data["badges_count"] = $badge_count;

		$data["nodes_count_percent"] = 0;
		$data["paths_count_percent"] = 0;
		$data["logins_count_percent"] = 0;
		$data["logouts_count_percent"] = 0;
		$data["badges_count_percent"] = 0;

		if ($node_count!=0)
			$data["nodes_count_percent"] = round(($node_count/$this->Activity_model->get_type_count_all("node_open")*100), 2);

		if ($path_count!=0)
			$data["paths_count_percent"] = round(($path_count/$this->Activity_model->get_type_count_all("path_activate")*100), 2);

		if ($login_count!=0)
			$data["logins_count_percent"] = round(($login_count/$this->Activity_model->get_type_count_all("login")*100), 2);

		if ($logout_count!=0)
			$data["logouts_count_percent"] = round(($logout_count/$this->Activity_model->get_type_count_all("logout")*100), 2);

		if ($badge_count!=0)
			$data["badges_count_percent"] = round(($badge_count/$this->Activity_model->get_type_count_all("badge")*100), 2);

		$data["total_players"] = count($this->User_model->get_all());

		$this->load->view('header', $data);
    	$this->load->view('navigation');
    	$this->load->view('dashboard');
		$this->load->view('footer');
	}


}
