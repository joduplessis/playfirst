<?php
header('Access-Control-Allow-Origin: *');
defined('BASEPATH') OR exit('No direct script access allowed');

class Management extends CI_Controller {
	public function __construct() {
		parent::__construct();

		$this->load->library('session');
	}

	public function logout() {
		$this->load->view('header');
		$this->load->view('logout');
		$this->load->view('footer');
	}

	public function login() {
		$this->load->view('header');
		$this->load->view('login');
		$this->load->view('footer');

		$this->load->model('Activity_model');
		$output = $this->Activity_model->get_all();
	}

	public function login_do() {
		$email = $this->input->post('email');
		$password = $this->input->post('password');

		$this->load->model('User_model');
		$output = $this->User_model->checkUser($email, $password);

		if ($output==0) {
			echo "<script> alert('Incorrect email or password.'); window.history.back(); </script>";
		} else {
			$user = $this->User_model->get_one($output);
			$this->session->set_userdata("user", $user);

			redirect('dashboard', 'location');
		}
	}
}
