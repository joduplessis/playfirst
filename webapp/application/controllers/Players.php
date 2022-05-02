<?php
header('Access-Control-Allow-Origin: *');
defined('BASEPATH') OR exit('No direct script access allowed');

class Players extends CI_Controller {

	public function __construct() {
		parent::__construct();
	}

	public function index() {
    $this->load->model('User_model');
		$this->load->model('Activity_model');

		$output = $this->User_model->get_all();
		$allLogins = $this->Activity_model->get_all_login_count();

		// Get the count for the placed
		foreach ($output as $player) {
			$logins = $this->Activity_model->get_user_login_count($player->id);
			$logins_percent = ($logins/$allLogins)*100;

			$player->{"logins"} = $logins;
			$player->{"logins_percent"} = round($logins_percent, 2);
		}

		$data["players"] = $output;
		$data["heading"] = "Players";

		$this->load->view('header');
		$this->load->view('navigation');
		$this->load->view('players', $data);
		$this->load->view('footer');
	}
}
