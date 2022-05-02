<?php
header('Access-Control-Allow-Origin: *');
defined('BASEPATH') OR exit('No direct script access allowed');

class Guides extends CI_Controller {
	public function __construct() {
		parent::__construct();
	}

	public function index() {
		$this->load->model('Guide_model');
		$this->load->model('Node_model');

		$output = $this->Guide_model->get_all();
		$output_nodes = $this->Node_model->get_all();

		foreach($output as $guide) {
			if ($guide->node==0) {
				$guide->{"node_title"} = "General guide";
			} else {
				$guide->{"node_title"} = $this->Node_model->get_one($guide->node)[0]->title;
			}
		}

		$data["guides"] = $output;
		$data["heading"] = "Guides";
		$data["nodes"] = $output_nodes;

		$this->load->view('header', $data);
		$this->load->view('navigation');
		$this->load->view('guides');
		$this->load->view('footer');
	}

	public function guide_add() {
    $this->load->model('Guide_model');

		$text = $this->input->post('text');
		$node = 0;

    $this->Guide_model->add($text, $node);

		redirect('guides', 'location');
  }

	public function guide_delete() {
		$id = $this->uri->segment(3);
    $this->load->model('Guide_model');
		$this->Guide_model->delete($id);

		redirect('guides?success="true"', 'location');
  }

	public function guide() {
		$id = $this->uri->segment(3);

  	$this->load->model('Guide_model');
		$this->load->model('Node_model');

		$output = $this->Guide_model->get_one($id);
		$output_nodes = $this->Node_model->get_all();

		$data["guide"] = $output[0];
		$data["title"] = "Guides";
		$data["nodes"] = $output_nodes;

		$this->load->view('header', $data);
		$this->load->view('navigation');
		$this->load->view('guide');
		$this->load->view('footer');
	}

	public function guide_update() {
		$this->load->model('Guide_model');

		$data['id'] = $_GET['id'];
		$data['text'] = $_GET['text'];
		$data['node'] = $_GET['node'];

		$this->Guide_model->update($data);
  }

}
