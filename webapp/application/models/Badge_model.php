<?php
class Badge_model extends CI_Model {

    public $image;
    public $title;
    public $description;
    public $node;
    public $points;

    public function __construct() {
        parent::__construct();
    }

    public function get_all() {
  		$query = $this->db->get('badges');
  		return $query->result();
    }

    public function get_one($id) {
  		$this->db->where('id', $id);
  		$query = $this->db->get('badges');
  		return $query->result();
    }

    public function add($title) {
        $this->title        = $title;
        $this->description  = "Brand new badge for the player";
        $this->image        = 'images/blank.gif';
        $this->node        = 0;
        $this->points        = 0;

        $this->db->insert('badges', $this);
    }

    public function delete($id) {
        $query = $this->db->delete('badges', array('id' => $id));
    }

    public function update($data) {
        $this->db->where('id', $data["id"]);
        return $this->db->update('badges', $data);
    }
}
?>
