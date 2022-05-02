<?php
header('Access-Control-Allow-Origin: *');
defined('BASEPATH') OR exit('No direct script access allowed');

class Worlds extends CI_Controller {
	public function __construct() {
		parent::__construct();
	}

	public function index() {
		$this->load->model('World_model');

		$output = $this->World_model->get_all();

		$data["worlds"] = $output;
		$data["heading"] = "Worlds";

		$this->load->view('header', $data);
		$this->load->view('navigation');
		$this->load->view('worlds');
		$this->load->view('footer');
	}

	public function world_add() {
    $this->load->model('World_model');

		$title = $this->input->post('title');

    $this->World_model->add($title);

		redirect('worlds', 'location');
  }

	public function world_delete() {
		$id = $this->uri->segment(3);
    $this->load->model('World_model');
		$this->World_model->delete($id);

		redirect('worlds?success=true', 'location');
  }

	public function world() {
		$id = $this->uri->segment(3);

  	$this->load->model('World_model');

		$output = $this->World_model->get_one($id);

		$data["world"] = $output[0];
		$data["title"] = "Worlds";

		$this->load->view('header', $data);
		$this->load->view('navigation');
		$this->load->view('world');
		$this->load->view('footer');
	}

	public function world_update() {
    $this->load->model('World_model');
    $this->World_model->update();

		redirect('worlds?success=true', 'location');
  }

}
